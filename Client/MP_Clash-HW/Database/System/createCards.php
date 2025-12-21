<?php
    require '../database.php';

    $cardsNames = array('Archer', 'Golem', 'Warrior', 'Warrior_Blue', 'Warrior_Green', 'Warrior_Orange', 'Heal');    

    foreach($cardsNames as $name){
        $card = R::dispense('cards');
        $card -> name = $name;
        R::store($card);
    }

?>