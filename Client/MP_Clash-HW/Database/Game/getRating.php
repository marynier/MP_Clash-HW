<?php
    require '../database.php';

    $userID = $_POST['userID'];

    $user = R::load('users', $userID);  
    $rating = $user -> fetchAs('ratings') -> rating;

    echo 'ok|'.$rating -> win.'|'.$rating -> loss;
?>