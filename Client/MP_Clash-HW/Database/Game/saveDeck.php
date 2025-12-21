<?php
require '../database.php';

$data = json_decode($_POST['data'], true);
$userID = $data['userID'] ?? null;
$selectedIDs = array_map('intval', $data['selectedIDs'] ?? []);

$user = R::load('users', $userID);
if (!$user -> id) {
    echo 'User not found';
    exit;
}

$allLinks = $user -> ownCardsUsers;
foreach($allLinks as $link) {
    $link -> selected = false;
}
R::store($user);

if (!empty($selectedIDs)) {
    $links = $user 
        -> withCondition('cards_users.cards_id IN ('. R::genSlots($selectedIDs) .')', $selectedIDs) 
        -> ownCardsUsers;
    
    foreach($links as $link) {
        $link->selected = true;
    }
}

R::store($user);
echo 'OK';

?>