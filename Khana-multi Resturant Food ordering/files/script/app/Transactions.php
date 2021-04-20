<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Transactions extends Model
{
    public function user()
    {
    	return $this->hasOne('App\User','id','user_id')->with('info');
    }

    public function admin()
    {
    	return $this->hasOne('App\User','id','admin_id')->with('info');
    }
}
