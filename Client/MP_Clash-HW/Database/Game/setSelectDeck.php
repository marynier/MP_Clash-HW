<?php
    require '../System/configConstants.php';
    require '../database.php';

    $userID = $_POST['userID'];
    $json = $_POST['json'];

     if(!isset($userID) || !isset($json)){
        echo 'Data struct error';
        exit;
    }

    $selectedIDs = json_decode($json, true)['IDs'];
    if(!isset($selectedIDs)){
        echo 'Array not found: ' . $json;
        exit;
    }

    $user = R::load('user', $userID);
    
    // $links = $user -> withCondition('cards_users.cards_id IN ('. R::genSlots($selectedIDs) .')', $selectedIDs) -> ownCardsUsers;
   
    // if(count($links) != DECK_SIZE) { //проверить количество связей
    //     echo 'Count error ' . count($links);
    //     exit;
    // }
   
    // foreach($links as $link){
    //     $link -> selected = true;
    // }
    // R::store($user);

    // $links = $user ->
    // withCondition('cards_users.cards_id NOT IN ('. R::genSlots($selectedIDs) .') AND cards_users.selected = ?', [...$selectedIDs, true]) -> ownCardsUsers;
   
    // foreach($links as $link){
    //     $link -> selected = false;
    // }
    // R::store($user);

    // echo 'ok';

    $links = R::find('cards_users', 'users_id = ? AND cards_id IN ('.R::genSlots($selectedIDs).')', 
        array_merge([$userID], $selectedIDs)
    );

    if(count($links) != DECK_SIZE) { 
        echo 'Count error ' . count($links); 
        exit;
    }

    foreach($links as $link){
        $link->selected = true;
    }
    R::storeAll($links);

    $notSelectedLinks = R::find('cards_users', 'users_id = ? AND cards_id NOT IN ('.R::genSlots($selectedIDs).') AND selected = ?', 
        array_merge([$userID], $selectedIDs, [true])
    );

    foreach($notSelectedLinks as $link){
        $link->selected = false;
    }
    R::storeAll($notSelectedLinks);

    echo 'ok';
?>