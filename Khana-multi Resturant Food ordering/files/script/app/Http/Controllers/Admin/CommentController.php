<?php

namespace App\Http\Controllers\Admin;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Comment;
use App\Options;
use Auth;
class CommentController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request)
    {
        
        if ($request->qry==1) {
             $comments=Comment::with('term')->where('status',1)->where('p_id',null)->latest()->paginate(20);
             $status=1;
             return view('admin.comment.index',compact('comments','status'));
        }
         if ($request->qry=='replyed') {
             $comments=Comment::with('term')->where('p_id',null)->where('status',4)->latest()->paginate(20);
             $status=4;
             return view('admin.comment.index',compact('comments','status'));
        }
         elseif ($request->qry=='adminreplyed') {
             $comments=Comment::with('term')->where('status',4)->latest()->paginate(20);
             $status=4;
             return view('admin.comment.index',compact('comments','status'));
        }

        elseif ($request->qry==2) {
             $comments=Comment::with('term')->where('status',2)->where('p_id',null)->latest()->paginate(20);
             $status=2;
            
             return view('admin.comment.index',compact('comments','status'));
        }
         elseif ($request->qry==3) {
             $comments=Comment::with('term')->where('status',3)->where('p_id',null)->latest()->paginate(20);
             $status=3;
             return view('admin.comment.index',compact('comments','status'));
        }
        elseif ($request->qry=='trash') {
             $comments=Comment::with('term')->where('status',0)->latest()->paginate(20);
             $status=0;
             return view('admin.comment.index',compact('comments','status'));
        }
        $status=1;
        $comments=Comment::with('term')->where('status','!=',0)->where('p_id',null)->latest()->paginate(20);
        return view('admin.comment.index',compact('comments','status'));
    }


    public function apiview()
    {
        return view('admin.comment.settings');
    }

    public function disqus(Request $request)
    {
        $option=Options::where('key','disqus_comment')->first();
        if (empty($option)) {
            $option= new Options;
           
        }
        $option->key="disqus_comment";
        $option->value=$request->disqus;
        $option->save();
        return response()->json(['Disqus comment configured successfully']);
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create()
    {
        //
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        $comments= Comment::find($request->id);
        $comments->status = 4;
        $comments->save();

        $comment=new Comment;
        $comment->term_id=$request->post_id;
        $comment->p_id=$request->id;
        $comment->auth_id=Auth::id();
        $comment->comment=$request->reply;
        $comment->status=4;
        $comment->save();
        return response()->json(['Reply Success']);
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
         $comments=Comment::with('reply')->find($id);
        
        
        return view('admin.comment.reply',compact('comments'));
       
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit($id)
    {
        //
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, $id)
    {
        //
    }


   

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy(Request $request)
    {
        
   
        if ($request->status != 'delete') {

            if ($request->ids) {
                foreach ($request->ids as $id) {
                    $comment=Comment::find($id);
                    $comment->status=$request->status;
                    $comment->Save();
                   
                }
            }
            return response()->json('Success');
        }

         if ($request->status=='delete') {
             if ($request->ids) {
                foreach ($request->ids as $id) {
                   Comment::where('p_id',$id)->delete();
                   Comment::destroy($id);
                   
                }
                return response()->json('Success');
            }
         }
            
       
    }
}
