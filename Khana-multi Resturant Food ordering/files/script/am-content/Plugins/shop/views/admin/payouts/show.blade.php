@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=> 'Payout'])

<div class="row">
  <div class="col-sm-4">
    <div class="card">
      <div class="card-header">
        <h4>{{ __('Request Information') }}</h4>
        @if($info->user->role_id==3)
        <a href="{{ route('admin.vendor.show',$info->user_id) }}" class="btn btn-primary btn-sm ml-5">Go to Profile</a>
        @endif
      </div>
      <form method="post" action="{{ route('admin.payout.update',$info->id) }}" id="basicform">
        @csrf
        <div class="card-body">
          <div class="form-group">
            <label>{{ __('Amount') }}</label>
            <input type="number" name="amount" value="{{ $info->amount }}" class="form-control" autocomplete="off">
          </div>
          <div class="form-group">
            <label>{{ __('Requested Method Is') }} <b>{{ strtoupper($info->payment_mode) }}</b></label>
            <select class="form-control" name="method">
             <option value="paypal" @if($info->payment_mode=='paypal') selected @endif>{{ __('Paypal') }}</option>
             <option value="bank-transfer" @if($info->payment_mode=='bank-transfer') selected @endif>{{ __('Bank Transfer') }}</option>
           </select>
         </div> 
         <div class="form-group">
          <label>{{ __('Status') }}</label>
          <select class="form-control" name="status">
            <option value="2" @if($info->status==2) selected="" @endif>{{ __('Pending') }}</option>
            <option value="1" @if($info->status==1) selected="" @endif>{{ __('Complete') }}</option>
            <option value="0" @if($info->status==0) selected="" @endif>{{ __('Cancel') }}</option>
          </select>
        </div> 

        @if($info->status==2)
        <button class="btn btn-primary col-12" type="submit">{{ __('Update') }}</button>
        @endif
      </div>

    </form>
  </div>
</div>

<div class="col-sm-4">
  <div class="card">
    <div class="card-header">
      <h4>{{ __('Transactional Information') }}</h4>
    </div>
    <div class="card-body">
      @if(!empty($paypal))
      <h6>{{ __('Paypal') }}</h6>
      {{ __('Transactional Email') }}:  {{ $paypal->content }}
      <hr>
      @endif
      @if(!empty($bank))
      <h6>{{ __('Bank Details') }}</h6>
      @php
      $json=json_decode($bank->content);
      @endphp
      <p>{{ __('Bank Name') }}:  {{ $json->bank_name }}</p>
      <p>{{ __('Branch Name') }}:  {{ $json->branch_name }}</p>
      <p>{{ __('Account Holder Name') }}:  {{ $json->holder_name }}</p>
      <p>{{ __('Account Number') }}:  {{ $json->account_number }}</p>
      @endif
    </div>

  </div>
</div>

<div class="col-sm-4">
  <div class="card">
    <div class="card-header">
      <h4>{{ __('Admin Information') }}</h4>
    </div>
    <div class="card-body">
      @if(!empty($info->admin))
      <p>{{ __('Name') }}: {{ $info->admin->name }}</p>
      <p>{{ __('Email') }}: {{ $info->admin->email }}</p>
      <p>{{ __('Updated At') }}: {{ $info->updated_at->diffforHumans() }}</p>
      @endif
    </div>
  </div>
</div>
<div class="col-sm-8">
  <div class="card">
    <div class="card-header">
      <h6>{{ __('Transection History') }}</h6>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <table class="table table-striped table-hover text-center table-borderless">
          <thead>
            <tr>
             <th>{{ __('Transaction ID') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Payout Method') }}</th>
              <th>{{ __('Date Processed') }}</th>
              <th>{{ __('Payout Status') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($transections as $row)
            <tr>
              <td>#{{ $row->id }}</td>
              <td>{{ $row->amount }}</td>
              <td>{{ strtoupper($row->payment_mode) }}</td>
              <td>{{ $row->created_at->format('Y-F-d') }}</td>
              <td>@if($row->status==0)
                <span class="badge badge-danger">{{ __('Canceled') }}</span>
                @elseif($row->status==1)
                <span class="badge badge-success">{{ __('Completed') }}</span> @elseif($row->status==2)
                <span class="badge badge-primary">
                  {{ __('Pending') }}

                @endif</td>
                  <td><a href="{{ route('admin.payout.show',$row->id) }}" class="btn btn-primary btn-sm"><i class="fas fa-eye"></i></a></td>
              </tr>
              @endforeach
          </tbody>
          
        </table>
        {{ $transections->links() }}

      </div>
    </div>
  </div>
</div>
<div class="col-sm-4">
  <div class="card">
    <div class="card-header">
      <h4>{{ __('Overview Of Earnings') }}</h4>
    </div>
    <div class="card-body">
   
      <ul class="list-group">
        <li class="list-group-item d-flex justify-content-between align-items-center">
          {{ __('Amount Of Sell') }}
          <span class="badge badge-primary badge-pill">{{ $total_amount }}</span>
          
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
          {{ __('Commission') }}
          <span class="badge badge-primary badge-pill">{{ $total_commission }}</span>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
          {{ __('Earnings') }}
          <span class="badge badge-primary badge-pill">{{ $total_amount-$total_commission }}</span>
        </li>
      </ul>
    
  </div>
</div>
</div>
</div>

@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script>
  "use strict";
  function success(param) {
   window.location.reload();
  }
</script>
@endsection