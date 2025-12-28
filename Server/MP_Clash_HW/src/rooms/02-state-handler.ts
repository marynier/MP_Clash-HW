import { Room, Client, Delayed } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";
import axios from 'axios';
import { Library } from "../Library";

export class Player extends Schema {   
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    createPlayer(sessionId: string) {
        this.players.set(sessionId, new Player());
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }    
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 2;
    playersDeck = new Map();    
    countdownInterval: Delayed;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            console.log("StateHandlerRoom received message from", client.sessionId, ":", data);
            //this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("SpawnUnit", (client, data) => {
            this.broadcast("SpawnUnit", data, { except: client });
        });
    }

    async onJoin (client: Client, data) {
        try {
            const response = await axios.post(Library.getDeckURI, {key: Library.phpKEY, userID: data.id});
            console.log(response.data);
            this.playersDeck.set(client.id, response.data);
        } catch (error) {
            console.log('Вылетела ошибка ' + error);
        }
        this.state.createPlayer(client.sessionId);

        if(this.clients.length < 2) return;

        this.startGame();     
    }    

    startGame() {
        try {            
            this.broadcast("StartGame", JSON.stringify({
                player1ID: this.clients[0].id, 
                player1: this.playersDeck.get(this.clients[0].id), 
                player2: this.playersDeck.get(this.clients[1].id)
            }));
            
            this.startInGameCountdown();
        } catch (error) {
            console.log('Ошибка при старте игры: ' + error);
            this.broadcast("CancelStart");
        }
    }

    startInGameCountdown() {
        let countdownValue = 10;        
        
        this.broadcast("CountdownStart", countdownValue);        
        
        this.countdownInterval = this.clock.setInterval(() => {
            countdownValue--;
            
            if (countdownValue > 0) {                
                this.broadcast("CountdownUpdate", countdownValue);
            } else {                
                this.broadcast("CountdownUpdate", 0);
                                
                if (this.countdownInterval) {
                    this.countdownInterval.clear();
                }
            }
        }, 1000);
    }

    onLeave (client) {
        if (this.countdownInterval) {
            this.countdownInterval.clear();
            this.broadcast("CancelStart");
        } 

        if(this.playersDeck.has(client.id))
            this.playersDeck.delete(client.id);

        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
    }
}
