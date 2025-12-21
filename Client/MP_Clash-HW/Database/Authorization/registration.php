<?php
    require '../database.php';

    $login = $_POST['login'];
    $password = $_POST['password'];

    if(!isset($login) || !isset($password)){
        echo 'Data struct error';
        exit;
    }

    $repeatChecker = R::findOne('users', 'login = ?', array($login));
    
    if(isset($repeatChecker)){
        echo 'Login reserved';
        exit;
    }

    $user = R::dispense('users');
    $user -> login = $login;
    $user -> password = $password;

/////////////////////////////////////////////////
    $availableCards = array(1,2,3,4,5,6,7);
    foreach($availableCards as $id){
        $card = R::load('cards', $id);
        $user -> link('cards_users', array('selected' => false)) -> cards = $card;
    }

    R::store($user);

    $selectedID = array(1,3,5,6);
    $links = $user -> withCondition('cards_users.cards_id IN ('. R::genSlots($selectedID) .')', $selectedID) -> ownCardsUsers; //ownCardsUsers - запрашиваем связывающую таблицу cards_users 
    
    foreach($links as $link){
        $link -> selected = true;
    }
//////////////////////////////////////////////
    R::store($user);

    echo 'ok';
?>