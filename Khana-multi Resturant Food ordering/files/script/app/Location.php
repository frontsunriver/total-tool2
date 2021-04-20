<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Location extends Model
{
      public function restaurant(){
         return $this->hasOne('App\User','id','user_id')->where('role_id',3)->where('status','approved')->select('id','slug','name')->with('avg_ratting','ratting','shopcategory','delivery','preview','coupons');
      }
      public function restaurant_info(){
         return $this->hasOne('App\User','id','user_id')->where('role_id',3)->where('status','approved')->select('id','slug','name')->with('avg_ratting','ratting','shoptag','delivery','preview','coupons');
      }
      
      public function Offerables()
      {
         return $this->hasOne('App\Terms','auth_id','user_id')->where('type',10)->where('status',1)->select('id','title','auth_id')->wherehas('restaurants')->with('restaurants');
      }

      public function users(){
   		return $this->hasOne('App\User','id','user_id')->where('role_id',3)->where('status','approved')->with('avg_ratting','ratting','shopcategory','delivery','preview','coupons');
   	}

   	public function categories(){
   		return $this->hasOne('App\Usercategory','user_id','user_id');
   	}

   	public function restaurants()
   	{
   		return $this->hasOne('App\User','id','user_id')->where('role_id',3)->with('avg_ratting','ratting','delivery','preview');
   	}

      public function area()
      {
         return $this->hasOne('App\Terms','id','term_id')->with('excerpt');
      }
      public function riders()
      {
         return $this->hasOne('App\User','id','user_id')->where('status','approved')->select('id','name','email');
      }

      public function coupons()
      {
        return $this->hasOne('App\Terms','id','auth_id')->where('type',10)->where('status',1);
      }

      public function city()
      {
         return $this->hasOne('App\Terms','id','term_id')->select('id','title','slug');
      }


}
