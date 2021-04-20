<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Featured extends Model
{
    protected $table="featured_user";

    public function rattings()
    {
    	return $this->hasOne('App\User','id','user_id')->with('avg_ratting','ratting');
    }

    public function users(){
   		return $this->hasOne('App\User','id','user_id')->where('status','approved')->where('role_id',3)->with('avg_ratting','preview','delivery','shopcategory','ratting');
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
