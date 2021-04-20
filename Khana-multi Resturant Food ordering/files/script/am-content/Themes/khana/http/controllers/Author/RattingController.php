<?php 

namespace Amcoders\Theme\khana\http\controllers\Author;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Auth;
use App\Comment;
use App\Commentmeta;
use App\User;
use App\Usermeta;
/**
 * 
 */
class RattingController extends controller
{
	public function review(Request $request)
	{
		$this->validate($request,[
			'ratting' => 'required',
			'review' => 'required'
		]);

		$review = new Comment();
		$review->order_id = $request->order_id;
		$review->user_id = Auth::User()->id;
		$review->vendor_id = $request->vendor_id;
		$review->save();

		$review_meta = new Commentmeta();
		$review_meta->comment_id = $review->id;
		$review_meta->vendor_id = $request->vendor_id;
		$review_meta->star_rate = $request->ratting;
		$review_meta->comment = $request->review;
		$review_meta->save();

		$store = User::where('id',$request->vendor_id)->first();

		$store_avg_ratting = $store->vendor_reviews->avg('comment_meta.star_rate');

		$usermeta_3 = Usermeta::where([
			['user_id',$request->vendor_id],
			['type','rattings']
		])->first();
		if($usermeta_3)
		{
			$usermeta_3->content = $store->vendor_reviews()->count();
        	$usermeta_3->save();
		}else{
			$usermeta_3 = new Usermeta();
	        $usermeta_3->user_id = $request->vendor_id;
	        $usermeta_3->type = 'rattings';
	        $usermeta_3->content = $store->vendor_reviews()->count();
	        $usermeta_3->save();
		}
        

        $usermeta_4 = Usermeta::where([
			['user_id',$request->vendor_id],
			['type','avg_rattings']
		])->first();
		if($usermeta_4)
		{
			$usermeta_4->content = number_format($store_avg_ratting,1);
        	$usermeta_4->save();
		}else{
			$usermeta_4 = new Usermeta();
	        $usermeta_4->user_id = $request->vendor_id;
	        $usermeta_4->type = 'avg_rattings';
	        $usermeta_4->content = number_format($store_avg_ratting,1);
	        $usermeta_4->save();
		}
        

		return back();
	}
}