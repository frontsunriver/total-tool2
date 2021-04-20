@extends('layouts.backend.app')

@section('style')
<link rel="stylesheet" href="{{ asset('admin/css/summernote.min.css') }}">
<link rel="stylesheet" href="{{ asset('admin/css/bootstrap-tagsinput.css') }}">
@endsection

@section('content')
<div class="row">
 <div class="col-lg-9">      
  <div class="card">
   <div class="card-body">
     <div class="alert alert-danger none errorarea">
      <ul id="errors">

      </ul>
    </div>
    <h4>{{ __('Add new Post') }}</h4>
    <form method="post" id="basicform" action="{{ route('admin.post.store') }}">
     @csrf
     <div class="custom-form pt-20">
      
     @php
     $arr['title']= 'Post Title';
     $arr['id']= 'name';
     $arr['type']= 'text';
     $arr['placeholder']= 'Post Title';
     $arr['name']= 'title';
     $arr['is_required'] = true;

     echo  input($arr);

     $arr['title']= 'Excerpt';
     $arr['id']= 'excerpt';
     $arr['placeholder']= 'short description';
     $arr['name']= 'excerpt';
     $arr['is_required'] = true;

     echo  textarea($arr);
     
     $arrn['title']= 'Post Content';
     $arrn['name']= 'content';
    
     echo  editor($arrn);
     @endphp
     
   </div>
 </div>
</div>

</div>
<div class="col-lg-3">
  <div class="single-area">
   <div class="card">
    <div class="card-body">
     <h5>{{ __('Publish') }}</h5>
     <hr>
     <div class="btn-publish">
      <button type="submit" class="btn btn-primary col-12"><i class="fa fa-save"></i> {{ __('Save') }}</button>
    </div>
  </div>
</div>
</div>
<div class="single-area">
 <div class="card sub">
  <div class="card-body">
   <h5>{{ __('Status') }}</h5>
   <hr>
   <select class="custom-select mr-sm-2" id="inlineFormCustomSelect" name="status">
    <option value="1">{{ __('Published') }}</option>
    <option value="2">{{ __('Draft') }}</option>

  </select>
</div>
</div>
</div>
<?php echo adminLang(); ?>
<div class="single-area">
 <div class="card sub">
  <div class="card-body">
   <h5>{{ __('Categories') }}</h5>
   <hr>
   <div class="scroll-bar-wrap">
     <div class="category-list">
       {{ AdminCategory(0) }}
       <div class="cover-bar"></div>
     </div>
   </div>
 </div>
</div>
</div>


{{ mediasection() }}


<div class="single-area">
  <div class="card sub">
    <div class="card-body">
      <h5>{{ __('Tags') }}</h5>
      <hr>
      <input type="text" name="tag" id="tags" class="form-control">
    </div>
  </div>
</div>
<div class="single-area">
  <div class="card sub">
    <div class="card-body">
      <h5>{{ __('Comment Status') }}</h5>
      <hr>
      <select class="form-control" name="comment_status">
        <option value="1">{{ __('Allowed Comment') }}</option>
        <option value="0">{{ __('Comment Not Allow') }}</option>
        
      </select>
    </div>
  </div>
</div>
</div>
</div>
<input type="hidden"  name="type" value="0">
<input type="hidden"  name="post_type" value="blog">
<button type="button" data-toggle="modal" data-target=".media-multiple" class="multi-modal" class="btn bg-btn" >test</button>


</form>

{{ mediasingle() }}
@include('admin.media.multiplemediamodel')
@endsection

@section('script')
<script src="{{ asset('admin/js/summernote.min.js') }}"></script>
<script src="{{ asset('admin/js/custom-summernote.min.js') }}"></script>
<script src="{{ asset('admin/js/bootstrap-tagsinput.min.js') }}"></script>
<script src="{{ asset('admin/js/form.js') }}"></script>
<script src="{{ asset('admin/js/media.js') }}"></script>
<script>
  (function ($) {
    "use strict";

    $('#tags').tagsinput();
    $('.use').on('click',function(){

      $('#preview').attr('src',myradiovalue);
      $('#preview_input').val(myradiovalue);
      
    });

    //for multiple
    $('.use1').on('click',function(){
      $('.multi-img').hide();
      $('.gallary-src').remove();
      $.each(mycheckboxvalue, function(index, value){
        $(".gallary-img").append('<img class="gallary-src" src="' + value + '" />');
      });

      $('#gallary_input').val(mycheckboxvalue.toString())

    });
})(jQuery);
//response will assign this function
function success(res){
  location.reload();
}
</script>
@endsection