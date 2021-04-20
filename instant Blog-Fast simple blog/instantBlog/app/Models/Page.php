<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Page extends Model
{
    use HasFactory;
    
	protected $guarded = ['id'];

    public $timestamps = false;

    //For firendly urls
    public function getRouteKeyName()
    {
        return 'page_slug';
    }

    //Convert url to slug format
    public function setPageSlugAttribute($value)
    {
        $this->attributes['page_slug'] = str_slug($value, '-');
    }
}
