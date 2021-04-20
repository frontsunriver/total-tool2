<?php 

namespace Amcoders\Theme\khana\http\controllers\Admin;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\Comment;
use Auth;
class ReviewController extends controller
{
	public function index()
	{
		if (!Auth()->user()->can('manage.review')) {
			return abort(401);
		}
		$reviews = Comment::orderBy('id','DESC')->with('comment_meta')->paginate(20);
		return view('theme::admin.review.index',compact('reviews'));
	}

	public function delete($id)
	{
		Comment::find($id)->delete();

		return back();
	}
}