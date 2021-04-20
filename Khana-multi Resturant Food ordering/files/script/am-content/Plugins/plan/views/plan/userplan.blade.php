@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=> $info->name.' Information'])

<div class="row">
  <div class="col-sm-4">
    <div class="card">
      <div class="card-header">
        <h5>{{ __('Information') }}</h5>
      </div>
      <form method="post" action="{{ route('admin.vendor.update',$info->id) }}" id="basicform">
        @csrf
        <div class="card-body">
          <div class="form-group">
            <label>{{ __('Name') }}</label>
            <input type="text" name="name" class="form-control" autocomplete="off" value="{{ $info->name }}" required="">
          </div>
          <div class="form-group">
            <label>{{ __('Email') }}</label>
            <input type="email" name="email" class="form-control" autocomplete="off" value="{{ $info->email }}" required>
          </div>
          
          <div class="form-group">
            <label>{{ __('Plan') }}</label>
            <select class="form-control" name="plan_id">
              @foreach($plans as $row)
              <option value="{{ $row->id }}" @if($info->plan_id==$row->id) selected="" @endif>{{ $row->name }}</option>
              @endforeach
            </select>
          </div>
          <div class="form-group">
            <label>{{ __('Status') }}</label>
            <select class="form-control" name="status">
             <option value="pending" @if($info->status=='pending') selected @endif>{{ __('Pending') }}</option>
             <option value="approved" @if($info->status=='approved') selected @endif>{{ __('Approved') }}</option>
             <option value="suspended" @if($info->status=='suspended') selected @endif>{{ __('Suspended') }}</option>
             <option value="cancelled" @if($info->status=='cancelled') selected @endif>{{ __('Cancelled') }}</option>
           </select>
         </div>
         
         <button class="btn btn-primary col-12" type="submit">{{ __('Update') }}</button>
       </div>
       
     </form>
   </div>
 </div>
 <div class="col-sm-8">
  <div class="card">
   <div class="card-header">
    <h5>{{ __('Other Information') }}</h5>
  </div>
  <div class="card-body">
    @php
    $json=json_decode($info->info->content);
    @endphp
    <p><b>{{ __('Support Email 1:') }} </b>{{ $json->email1 ?? '' }}</p>
    <p><b>{{ __('Support Email 2:') }} </b>{{ $json->email2 ?? '' }}</p>
    <p><b>{{ __('Support Phone 1:') }} </b>{{ $json->phone1 ?? '' }}</p>
    <p><b>{{ __('Support Phone 2:') }} </b>{{ $json->phone2 ?? '' }}</p>
    <p><b>{{ __('City:') }} </b>{{ $info->resturentlocationwithcity->area->title }}</p>
    <p><b>{{ __('Address line:') }} </b>{{ $json->address_line ?? '' }}</p>
    <p><b>{{ __('Location line:') }} </b>{{ $json->full_address ?? '' }}</p>
    <p><b>{{ __('Subscription Plan :') }} </b><a href="{{ route('admin.plan.edit', $info->usersaas->id ?? '') }}">{{ $info->usersaas->name ?? '' }}</a></p>
 </div>
</div>
</div>
<div class="col-md-12">
  <div class="card">
    <div class="card-header">
      <h6>{{ __('Purchase History') }}</h6>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <table class="table table-striped table-hover text-center table-borderless">
          <thead>
            <tr>
              <th>{{ __('Plan Name') }}</th>
              <th>{{ __('Payment Method') }}</th>
              <th>{{ __('Payment Status') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Ordered At') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($transections as $row)
            <tr>
              <td>{{ $row->usersaas->name }}</td>
              <td>{{ strtoupper($row->payment_method) }}</td>
               <td>
              @if($row->status == 1) 
                <span class="badge badge-success">{{ __('Completed') }}</span>
                              
                 @elseif($row->status == 0)  <span class="badge badge-danger"> {{ __('Pending') }} </span> 
               @endif
             </td>
               <td>
              @if($row->status == 1) 
                <span class="badge badge-success">{{ __('Completed') }}</span>
                 @elseif($row->status == 2) 
                 <span class="badge badge-primary"> {{ __('Pending') }} </span>
                 @elseif($row->status == 3) <span class="badge badge-warning"> {{ __('Accepted') }} </span> 
                 @elseif($row->status == 0)  <span class="badge badge-danger"> {{ __('Cancelled') }} </span> 
               @endif
             </td>
              <td>{{ $row->amount }}</td>
              <td>{{ $row->created_at->diffforHumans() }}</td>
            </tr>
            @endforeach
          </tbody>
          <tfoot>
            <tr>
              <th>{{ __('Plan Name') }}</th>
              <th>{{ __('Payment Method') }}</th>
              <th>{{ __('Payment Status') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Ordered At') }}</th>
            </tr>
          </tfoot>
        </table>
        {{ $transections->links() }}

      </div>
    </div>
  </div>
</div>
</div>
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
@endsection