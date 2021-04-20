@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=> $info->name.' Information'])

<div class="row">
  <div class="col-sm-12">
    <div class="card">
      <div class="card-header">
        <h5>{{ __('Primary Information') }}</h5>
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
            <label>{{ __('Password') }}</label>
            <input type="password" name="password" class="form-control" autocomplete="off" minlength="8">
            
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
         <div class="form-group">

           <input type="checkbox" name="password_change" value="1" id="check">
           <label for="check"><span class="text-danger">{{ __('With Change Password') }}</span></label>
         </div>
         <button class="btn btn-primary col-12" type="submit">{{ __('Update') }}</button>
       </div>
       
     </form>
   </div>
 </div>
<div class="col-sm-12">
  <div class="card">
    <div class="card-header">
      <h6>{{ __('Order History') }}</h6>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <table class="table table-striped table-hover text-center table-borderless">
          <thead>
            <tr>
              <th>{{ __('Order ID') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Method') }}</th>
              <th>{{ __('Payment Status') }}</th>
              <th>{{ __('Created at') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($orders as $row)

            <tr>
              <td><a href="{{ route('admin.order.details',$row->id) }}">#{{ $row->id }}</a></td>
               <td>
              @if($row->status == 1) 
                <span class="badge badge-success">{{ __('Completed') }}</span>
                 @elseif($row->status == 2) 
                 <span class="badge badge-primary"> {{ __('Pending') }} </span>
                 @elseif($row->status == 3) <span class="badge badge-warning"> {{ __('Accepted') }} </span> 
                 @elseif($row->status == 0)  <span class="badge badge-danger"> {{ __('Cancelled') }} </span> 
               @endif
             </td>
             
              <td>{{ $row->total+$row->shipping }}</td>
              <td>{{ strtoupper($row->payment_method) }}</td>           
              <td>@if($row->payment_status == 1)
                <span class="badge badge-success"> {{ __('Completed') }} </span> 
                @else
                <span class="badge badge-danger"> {{ __('pending') }} </span> 

                @endif</td>           
              <td>{{ $row->created_at->diffforHumans() }}</td>
              <td><a href="{{ route('admin.order.details',$row->id) }}" class="btn btn-primary"><i class="fas fa-eye"></i></a></td>
            </tr>
            @endforeach
          </tbody>
          <tfoot>
            <tr>
              <th>{{ __('Order ID') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Method') }}</th>
              <th>{{ __('Payment Status') }}</th>
              
              <th>{{ __('Created at') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </tfoot>
        </table>
        {{ $orders->links() }}

      </div>
    </div>
  </div>
</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
@endsection