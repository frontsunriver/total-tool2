<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Usermeta extends Model
{
   protected $table="user_meta";
   protected $fillable = [
        'user_id',
    ];
   	

   	public function users(){
   		return $this->hasOne('App\User','id','user_id')->where('status','approved')->where('role_id',3)->with('avg_ratting','preview','delivery','city');
   	}

   	public function coupons()
   	{
   		 return $this->hasOne('App\Terms','auth_id','user_id')->where('type',10)->where('status',1)->select('title','auth_id','count');
   	}

    public function locations()
    {
        return $this->hasOne('App\Location','user_id','user_id');;
    }
}
