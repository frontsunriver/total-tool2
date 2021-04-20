<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Userplan extends Model
{
   public function usersaas()
   {
   	return $this->hasOne('App\Plan','id','plan_id');
   }
   public function user()
   {
   	return $this->hasOne('App\User','id','user_id');
   }
}
