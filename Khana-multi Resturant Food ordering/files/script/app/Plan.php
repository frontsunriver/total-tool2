<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Plan extends Model
{
   protected $table="plan_meta";

   public function countusers()
   {
   	 return $this->hasMany('App\User','plan_id','id')->where('role_id',3);
   }

   public function user()
   {
   	return $this->hasOne('App\User','plan_id','id')->where('role_id',3);
   }
}
