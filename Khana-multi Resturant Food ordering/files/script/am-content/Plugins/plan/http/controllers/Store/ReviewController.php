<?php 

namespace Amcoders\Plugin\plan\http\controllers\Store;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Comment;
use Auth;
/**
 * 
 */
class ReviewController extends controller
{
	public function index()
	{
		$reviews = Comment::where('vendor_id',Auth::User()->id)->with('comment_meta')->paginate(20);
		return view('plugin::store.review.index',compact('reviews'));
	}
}