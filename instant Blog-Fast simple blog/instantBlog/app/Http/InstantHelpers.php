<?php

function shortNumber($value)
{
    $max = number_format($value, 0, ',', '.');
    $input_count = substr_count($max, '.');
    if ($input_count != '0') {
        if ($input_count == '1') {
            return substr($max, 0, -2).'K';
        } elseif ($input_count == '2') {
            return substr($max, 0, -6).'M';
        }
    } else {
        return $max;
    }
}

function levelNumber($value)
{

    if ($value < '100') {
        return $value = '1';
    } else {
        return substr($value, 0, -2);
    }
}
