@extends('layouts.backend.app')
@section('content')
<div class="row">
 <div class="col-lg-9">      
  <div class="card">
   <div class="card-body">
   <div class="row">
      <p class="text-danger text-left col-6">Before Update Your Site. Please Take A Backup</p>
      @php
      $version=App\Options::where('key','version')->first();

      @endphp
      @if(!empty($version))
     <p class="text-success text-right col-6">You are useing <b>{{ $version->value }}</b></p>
     @endif
   </div>
    <h4>{{ __('Site Update') }}  <div class="spinner-border text-success none" role="status" id="spinner">
      <span class="sr-only">Loading...</span>
    </div></h4>
    <h6 class="text-danger none" id="danger">Somethng Wrong</h6>
    <h6 class="text-success none" id="success">Site Update Successfully</h6>
    <form method="post" action="{{ route('admin.update.store') }}" id="update_form" enctype="multipart/form-data">
     @csrf
     <div class="custom-form pt-20">

       @php
       $arr['title']= 'Upload zip file';
       $arr['id']= 'file';
       $arr['type']= 'file';
       $arr['placeholder']= '';
       $arr['name']= 'file';
       $arr['is_required'] = true;
       echo  input($arr);       
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
      <button  type="submit" class="btn btn-primary col-12" id="submit"><i class="fa fa-save"></i> {{ __('Update') }}</button>
    </div>
  </div>
</div>
</div>

</div>
</div>
</form>
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script>
  "use strict";
  $('#update_form').on('submit',function(e){
    e.preventDefault();
    $.ajaxSetup({
      headers: {
        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
      }
    });

    $.ajax({

      url: this.action,
      data: new FormData(this),
      type: "POST",
      dataType: "json",
      processData: false,
      contentType: false,
      beforeSend: function() {
         $('#success').hide();
        $('#submit').attr('disabled','');
        $('#spinner').show();
        Sweet('error','Please Wait Some Moment',7000);
      },
      success: function(response) {
        
        $('#success').show();
        $('#spinner').hide();
        $('#danger').hide();
        $('#update_form').trigger("reset");
         Sweet('success','Site Update Successfully',2000);

      },
      error: function(xhr, status, error) {
        $('#success').hide();
        $('#submit').removeAttr('disabled');
        $('#spinner').hide();
        $('#danger').show();
        $.each(xhr.responseJSON.errors, function (key, item) 
        {
          Sweet('error',item)
        });
        $('#update_form').trigger("reset");

       
      }
    });
  });

</script>
@endsection