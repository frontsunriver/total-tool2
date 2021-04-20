@extends('layouts.backend.app')

@section('style')
@endsection

@section('content')
<div class="row">
  <div class="col-lg-9">      
    <div class="card">
      <div class="card-body">
        <h4>{{ __('Add new seller badge') }}</h4>
        <form method="post" action="{{ route('admin.badge.store') }}" id="basicform">
          @csrf
          <input type="hidden" name="type" value="3">
          <div class="pt-20">
            @php
            $arr['title']= 'Badge Name';
            $arr['id']= 'title';
            $arr['type']= 'text';
            $arr['placeholder']= 'Enter Name';
            $arr['name']= 'title';
            $arr['is_required'] = true;

            echo  input($arr);


            $arr['title']= 'Enter amount of earning after get the badge';
            $arr['id']= 'earning';
            $arr['type']= 'number';
            $arr['placeholder']= 'Enter amount';
            $arr['name']= 'number';
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
              <option selected value="1">{{ __('Published') }}</option>
              <option value="2">{{ __('Draft') }}</option>

            </select>
          </div>
        </div>
      </div>
      <div class="single-area">
        <div class="card sub">
          <div class="card-body">
            <h5>{{ __('Is Default ?') }}</h5>
            <hr>
            <select class="custom-select mr-sm-2"  name="default">
              <option  value="1">{{ __('Yes') }}</option>
              <option value="0" selected>{{ __('No') }}</option>
            </select>
          </div>
        </div>
      </div>

      {{ mediasection(array('title'=>'Badge Image')) }}


    </div>

  </form>

  {{ mediasingle() }}
  @endsection
  @section('script')
 
  <script src="{{ asset('admin/js/form.js') }}"></script>
  <script src="{{ asset('admin/js/media.js') }}"></script>

  <script>
    "use strict";
    (function ($) {
     

      $('.use').on('click',function(){

        $('#preview').attr('src',myradiovalue);
        $('#preview_input').val(myradiovalue);

      });

    })(jQuery);

  //success response will assign here
  function success(res){
    location.reload()
  } 
</script>
@endsection
