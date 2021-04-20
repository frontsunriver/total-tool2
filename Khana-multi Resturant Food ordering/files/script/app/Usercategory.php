<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Usercategory extends Model
{
    protected $table="user_category";

     public function users(){
   		return $this->hasOne('App\User','id','user_id')->where('role_id',3)->where('status','approved')->with('avg_ratting','ratting','shopcategory','delivery','preview');
   	}

}
