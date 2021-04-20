@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=>'Payout History'])
<div class="row">
  <div class="col-12 mt-2">
    <div class="card">
      <div class="card-body">
        @php
        $paid=\App\Transactions::where('status',1)->sum('amount');
        $pending=\App\Transactions::where('status',2)->sum('amount');
        $cancel=\App\Transactions::where('status',0)->sum('amount');
        @endphp
      <div class="float-left">
        <button class="btn btn-success" type="button">{{ __('Total Paid Of Amount '.number_format($paid)) }}</button>
         <button class="btn btn-warning" type="button">{{ __('Total Pending Of Amount '.number_format($pending)) }}</button>
         <button class="btn btn-danger" type="button">{{ __('Total Cancel Of Amount '.number_format($cancel)) }}</button>
       </div> 
      <div class="float-right">
        <form>
          <div class="input-group mb-2 col-12">

            <input type="number" class="form-control" placeholder="Search..." required="" name="src" autocomplete="off" value="{{ $src ?? '' }}" >
            <select class="form-control" name="type">
              <option value="user_id">{{ __('Search By User Id') }}</option>
              <option value="id">{{ __('Search By Transaction Id') }}</option>
            </select>
            <div class="input-group-append">                                            
              <button class="btn btn-primary" type="submit"><i class="fas fa-search"></i></button>
            </div>
          </div>
        </form>
      </div>
      <div class="table-responsive">
        <table class="table table-striped table-hover text-center table-borderless">
          <thead>
            <tr>
              <th>{{ __('Transaction Id') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Method') }}</th>
              <th>{{ __('Payment Status') }}</th>
                           
              <th>{{ __('Requested At') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($requests as $row)
            <tr>
              <td><a href="{{ route('admin.payout.show',$row->id) }}">#{{ $row->id }}</a></td>
              <td>{{ number_format($row->amount) }}</td>
              <td>{{ $row->payment_mode }}</td>
              <td>@if($row->status==0)
                <span class="badge badge-danger">{{ __('Canceled') }}</span>
                @elseif($row->status==1)
                <span class="badge badge-success">{{ __('Completed') }}</span> @elseif($row->status==2)
                <span class="badge badge-primary">
                  {{ __('Pending') }}

                @endif</td>
              <td>{{ $row->created_at->diffforHumans() }}</td>
              <td><a href="{{ route('admin.payout.show',$row->id) }}" class="btn btn-primary btn-sm"><i class="fas fa-eye"></i></a></td>
            </tr>
            @endforeach
          </tbody>
          <tfoot>
            <tr>
              <th>{{ __('Transaction Id') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Method') }}</th>
               <th>{{ __('Payment Status') }}</th>            
              <th>{{ __('Requested At') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </tfoot>
        </table>
        {{ $requests->links() }}
      </div>
    </div>
  </div>
</div>
</div>
@endsection
