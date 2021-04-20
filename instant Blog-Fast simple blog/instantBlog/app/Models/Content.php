<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Content extends Model
{
    use HasFactory;
    
    public $table = "contents";
    protected $casts = [
      'content' => 'array',
    ];
    protected $fillable = [
        'post_id', 'embed_id', 'type', 'body'
    ];
    public $timestamps = false;

    public function embed()
    {
        return $this->hasOne('App\Models\Embed', 'id', 'embed_id');
    }
}
