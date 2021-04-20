<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Image;
use Validator;

class FileUploadController extends Controller
{
    public function postImage(Request $request)
    {
        $validator = Validator::make($request->all(), [
            'post_image' => 'mimes:jpeg,png,jpg,gif,svg|max:2048',
        ]);

        //Check for image
        if ($validator->passes()) {
            $postimage = $request->file('post_image');

            if ($postimage->getClientOriginalExtension() == 'gif') {
                $filename = time() . '.' . $postimage->getClientOriginalExtension();
                $upload_success = $postimage->move(public_path('/uploads/'), $filename);
            } else {
                $filename = time() . '.' . $postimage->getClientOriginalExtension();
                $upload_success = Image::make($postimage)->resize(860, null, function ($constraint) {
                    $constraint->aspectRatio();
                })->save(public_path('/uploads/'. $filename));
            }

            if ($upload_success) {
                return response()->json(['success'=>$filename]);
            }
        }

        return response()->json(['error'=>$validator->errors()->all()]);
    }

    public function deleteFile(Request $request)
    {
        $delid = $request->id;
        $filename = public_path().'/uploads/'.$delid;
        $delete_success = \File::delete($filename);

        if ($delete_success) {
            return response()->json(['success'=>__('messages.form.imgremoved')]);
        } else {
            return response()->json(['error'=>__('messages.form.imgnot')]);
        }
    }
}
