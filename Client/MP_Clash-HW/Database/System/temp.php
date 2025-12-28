<?php
    require '../database.php';

    $user = R::load('users', 7);

    $rating = R::dispense('ratings');
    $rating -> win = 0;
    $rating -> loss = 0;

    $user -> rating = $rating;

    R::store($user);
    echo 'Complete';
?>