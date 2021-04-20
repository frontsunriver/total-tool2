<?php 

namespace Amcoders\Theme\khana\http\controllers;
use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use App\User;
use App\Terms;
use App\Usermeta;
use App\Featured;
use App\Options;
use Session;
use Artesaos\SEOTools\Facades\SEOMeta;
use Artesaos\SEOTools\Facades\OpenGraph;
use Artesaos\SEOTools\Facades\TwitterCard;
use Artesaos\SEOTools\Facades\JsonLd;
use Amcoders\Plugin\contactform\Contact;
use DB;

class WelcomeController extends controller
{
    
     /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {

       try {
          DB::connection()->getPdo();
          if(DB::connection()->getDatabaseName()){
           $top_rated= Usermeta::wherehas('users')->where('type','rattings')->wherehas('locations')->orderBy('content','DESC')->with('users','coupons')->take(8)->get()->map(
                function($data){
                    $qry= array();
                    $qry['city']= $data->users->city->city ?? '';
                    $qry['rattings']= $data->content;
                    $qry['slug']= $data->users->slug;
                    $qry['name']= $data->users->name;
                    $qry['avg_ratting']= number_format($data->users->avg_ratting->content ?? 0,1);
                    $qry['coupon']= $data->coupons;
                    $qry['time']= $data->users->delivery->content ?? null;
                    
                    
                    if (!empty($data->users->preview->content)) {
                       $qry['avatar']= ImageSize($data->users->preview->content,'medium');
                       
                    }
                    else{
                        $qry['avatar']= $data->users->avatar;
                    }



                    return $qry;
                }
            ) ;
           $features_resturent=Featured::where('type','featured_seller')->wherehas('users')->wherehas('locations')->with('users','coupons')->latest()->inRandomOrder()->take(8)->get()->map(function($data){

                    $qry= array();
                    $qry['city']=  $qry['city']= $data->users->city->city ?? '';;
                    $qry['rattings']= $data->users->ratting->content ?? 0;
                    $qry['slug']= $data->users->slug;
                    $qry['name']= $data->users->name;
                    $qry['avg_ratting']= number_format($data->users->avg_ratting->content ?? 0,1);
                    $qry['coupon']= $data->coupons;
                    $qry['time']= $data->users->delivery->content ?? null;
                    
                    
                    if (!empty($data->users->preview->content)) {
                       $qry['avatar']= ImageSize($data->users->preview->content,'medium');
                    }
                    else{
                        $qry['avatar']= $data->users->avatar;
                    }



                    return $qry;
           });

            $locations=Terms::where('type',2)->withcount('Locationcount')->with('preview')->latest()->get()->map(function($data){
                $qry['title']=$data->title;
                $qry['slug']=$data->slug;
                $qry['count']=$data->locationcount_count;
                $qry['preview']=$data->preview->content;
                return $qry;
            });

            $offer_resturents = Terms::where([
                ['type',10],
                ['status',1]
            ])->inRandomOrder()->wherehas('userwithpreview')->with('userwithpreview')->take(8)->get();
            
            $seo=Options::where('key','seo')->first();
            $seo=json_decode($seo->value);

            SEOMeta::setTitle($seo->title);
            SEOMeta::setDescription($seo->description);
            SEOMeta::setCanonical($seo->canonical);

            OpenGraph::setDescription($seo->description);
            OpenGraph::setTitle($seo->title);
            OpenGraph::setUrl($seo->canonical);
            OpenGraph::addProperty('keywords', $seo->tags);

            TwitterCard::setTitle($seo->title);
            TwitterCard::setSite($seo->twitterTitle);

            JsonLd::setTitle($seo->title);
            JsonLd::setDescription($seo->description);
            JsonLd::addImage(content('header','logo'));
            
            $color = Options::where('key','color')->first();
            if($color)
            {
                $theme_color = $color->value;
            }else{
                $theme_color = '#FF3252';
            }

            return view('theme::welcome.home',compact('top_rated','features_resturent','locations','offer_resturents','theme_color'));
          }else{
            return redirect()->route('install'); 
          }
        } catch (\Exception $e) {
            return redirect()->route('install'); 
        } 
    }

    public function topresturent()
    {
        $featured_resturents= Usermeta::where('type','rattings')->orderBy('content','DESC')->with('users')->take(8)->get();
       // dd($featured_resturents);
        return view('theme::welcome.section',compact('featured_resturents'));
    }

    public function notify()
    {
        Session::put('header_notify',[
            'status' => 'ok'
        ]);

        return Session::get('header_notify');
    }

    public function page($slug)
    {
        $info=Terms::where('type',1)->where('status',1)->where('slug',$slug)->with('excerpt','content')->first();
        if (empty($info)) {
            abort(404);
        }

        SEOMeta::setTitle($info->title);
        SEOMeta::setDescription($info->excerpt->content);

        OpenGraph::setDescription($info->excerpt->content);
        OpenGraph::setTitle($info->title);
        OpenGraph::setUrl(url()->current());
       

        TwitterCard::setTitle($info->title);
        TwitterCard::setSite($info->title);

        JsonLd::setTitle($info->title);
        JsonLd::setDescription($info->excerpt->content);
        JsonLd::addImage(content('header','logo'));
        return view('theme::welcome.page',compact('info'));
    }


    public function contact(Request $request)
    {
        
        Contact::send($request->name,$request->email,$request->subject,$request->message);

        return response()->json('ok');
    }
   
}