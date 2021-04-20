@extends('layouts.backend.app')

@section('content')
<div class="row">
  <div class="col-lg-9">      
    <div class="card">
      <div class="card-body">
        <h4>{{ __('Edit Plan') }}</h4>
        <form method="post" action="{{ route('admin.plan.update',$info->id) }}" id="basicform">
          @csrf
          @method('PUT')
          <div class="pt-20">
            @php
            $arr['title']= 'Plan Name';
            $arr['id']= 'name';
            $arr['type']= 'text';
            $arr['placeholder']= 'Enter Name';
            $arr['name']= 'name';
            $arr['value']= $info->name;
            $arr['is_required'] = true;

            echo  input($arr);

            $arr2['title']= 'Selling Price';
            $arr2['id']= 's_price';
            $arr2['type']= 'number';
            $arr2['placeholder']= 'Enter Selling Price';
            $arr2['name']= 's_price';
            $arr2['value']= $info->s_price;
            $arr['is_required'] = true;

            echo  input($arr2);

            $arr23['title']= 'Percentage of commission';
            $arr23['id']= 'commission';
            $arr23['type']= 'number';
            $arr23['placeholder']= 'Enter commission for per sale';
            $arr23['name']= 'commission';
            $arr3['is_required'] = true;
            $arr23['value']= $info->commission;
            echo  input($arr23);

            $arr2['title']= 'Image Upload Limit';
            $arr2['id']= 'img_limit';
            $arr2['type']= 'number';
            $arr2['placeholder']= 'Enter Image Limit';
            $arr2['name']= 'img_limit';
            $arr2['value']= $info->img_limit;
            $arr['is_required'] = true;

            echo  input($arr2);
            @endphp

            <div class="form-group">
              <label for="title">{{ __('Duration') }}<span class="text-danger"><b>*</b></span></label>
              <select name="duration" class="form-control">
                <option value="month" @if($info->duration=='month') selected="" @endif>Monthly</option>
                <option value="year" @if($info->duration=='year') selected="" @endif>Yearly</option>
              </select>
            </div>
            
            <div class="form-group">
              <label for="title">{{ __('Featured Resturent') }}</label>
              <select name="f_resturent" class="form-control" >
                <option value="1" @if($info->f_resturent==1) selected @endif>Enable</option>
                <option value="0"  @if($info->f_resturent==0) selected @endif>Disable</option>
              </select>
            </div>
             <div class="form-group">
              <label for="title">{{ __('Table Booking') }}</label>
              <select name="table_book" class="form-control" >
                <option value="1" @if($info->table_book == 1) selected="" @endif>Enable</option>
                <option value="0" @if($info->table_book == 0) selected="" @endif>Disable</option>
              </select>
            </div>
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
              <option selected value="1" @if($info->status==1) selected="" @endif>{{ __('Published') }}</option>
              <option value="2" @if($info->status==2) selected="" @endif>{{ __('Draft') }}</option>

            </select>
          </div>
        </div>
      </div>
   </div>
 </form>

@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
@endsection
