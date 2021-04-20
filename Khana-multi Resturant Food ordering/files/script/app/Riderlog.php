<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Riderlog extends Model
{
	public function pendingorders()
	{
		return $this->hasOne('App\Order','id','order_id')->with('resturentinfo')->select('id','rider_id','shipping','vendor_id','total');
	}

	public function user()
	{
		return $this->hasOne('App\User','id','user_id')->select('id','name');
	}

	public function completed()
	{
	  return $this->hasOne('App\Order','id','order_id')->where('status',1)->where('payment_status',1);
	}

	public function orders()
	{
		return $this->hasOne('App\Order','id','order_id')->select('id','total','shipping','status');
	}
}
