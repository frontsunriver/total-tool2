<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Comment extends Model
{
	protected $table="comments";
	public function term()
	{
		return $this->belongsTo('App\Terms');
	}

	public function reply()
	{
		return $this->hasMany('App\Comment','p_id','id');
	}

	public function comment_meta()
	{
		return $this->belongsTo('App\Commentmeta','id','comment_id');
	}
}
