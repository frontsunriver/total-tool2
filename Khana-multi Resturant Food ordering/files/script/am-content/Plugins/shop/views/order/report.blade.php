@extends('layouts.backend.app')
@section('content')
<div class="row">

  <div class="col-12">
    <div class="card mb-0">
      <div class="card-body">
        <ul class="nav nav-pills">
          <li class="nav-item">
            <a class="nav-link @if(url()->full() ==  route('store.order.index')) active @endif" href="{{ route('store.order.index') }}">{{ __('All Orders') }}<span class="badge @if(url()->full() ==  route('store.order.index')) badge-white @else badge-info @endif">{{ $allorders ?? \App\Order::where('vendor_id',Auth::id())->count() }}</span></a>
          </li>
          <li class="nav-item">
            <a class="nav-link @if(url()->full() ==  route('store.order.index','status=3')) active @endif" href="{{ route('store.order.index','status=3') }}">{{ __('Accepted Orders') }}<span class="badge badge-primary">{{ $accepted ??  \App\Order::where('vendor_id',Auth::id())->where('status',3)->count() }}</span></a>
          </li>
          <li class="nav-item">
            <a class="nav-link @if(url()->full() ==  route('store.order.index','status=2')) active @endif" href="{{ route('store.order.index','status=2') }}">{{ __('Pending OrdersOrders') }}<span class="badge badge-warning">{{ $pendings ?? \App\Order::where('vendor_id',Auth::id())->where('status',2)->count() }}</span></a>
          </li>
          <li class="nav-item">
            <a class="nav-link @if(url()->full() ==  route('store.order.index','status=0')) active @endif" href="{{ route('store.order.index','status=0') }}">{{ __('Declined Orders') }}<span class="badge badge-danger">{{ $declineds ?? \App\Order::where('vendor_id',Auth::id())->where('status',0)->count() }}</span></a>
          </li>
          <li class="nav-item @if(url()->full() ==  route('store.order.index','status=1')) active @endif">
            <a class="nav-link" href="{{ route('store.order.index','status=1') }}">{{ __('Completed Orders') }}<span class="badge badge-success">{{ $completed ?? \App\Order::where('vendor_id',Auth::id())->where('status',1)->count() }}</span></a>
          </li>
        </ul>
      </div>
    </div>
  </div>
  <div class="col-12 mt-2">
    <div class="card">
      <div class="card-body">
        <div class="float-left">
          <form>

            <div class="row">

              <div class="form-group ml-3">
                <label>{{ __('Starting Date') }}</label>
                <input type="date" name="start" class="form-control" required value="{{ $start ?? '' }}">
              </div>
              <div class="form-group ml-2">
               <label>{{ __('Starting Date') }}</label>
               <input type="date" name="end" class="form-control" required value="{{ $end ?? '' }}">
             </div>
             <div class="form-group ml-2">
               <label>{{ __('Status') }}</label>
               <select class="form-control" name="status">
                 <option value="" @if(empty($st ?? '')) selected @endif>{{ __('All') }}</option>
                 <option value="3" @if($st ?? '' == 3) selected @endif>{{ __('Accept') }}</option>
                 <option value="2" @if($st ?? '' == 2) selected @endif>{{ __('Pending') }}</option>
                 <option value="0" @if($st ?? '' == 0) selected @endif>{{ __('Declined') }}</option>
                 <option value="1" @if($st ?? '' == 1) selected @endif>{{ __('Completed') }}</option>
               </select>
             </div>

             <div class="form-group mt-4">
              <button class="btn btn-primary btn-lg  ml-2 mt-1" type="submit">{{ __('Filter') }}</button>
            </div>
          </div>
        </form>
      </div>
      <div class="float-right">
        <form>
          <div class="input-group mt-3 col-12">

            <input type="text" class="form-control" placeholder="Search By Order ID" required="" name="src" value="{{ $src ?? '' }}">
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
              <th>{{ __('Order ID') }}</th>
              <th>{{ __('Order Type') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Discount') }}</th>
              <th>{{ __('Commission') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Payment Status') }}</th>
              <th>{{ __('Time') }}</th>
              <th>{{ __('Action') }}</th>

            </tr>
          </thead>
          <tbody>
            @foreach($orders as $key => $row )
            <tr>
              <td><a href="{{ route('store.order.show',$row->id) }}">#{{ $row->id }}</a></td>
              <td>@if($row->order_type == 1) {{ __('Home Delivery') }} @else {{ __('Pickup') }} @endif</td>
              <td>{{ $row->total }}</td>
              <td>{{ $row->discount }}</td>
              <td>{{ $row->commission }}</td>
              <td>@if($row->status == 1) <span class="badge badge-success">{{ __('Completed') }}</span> @elseif($row->status == 2) <span class="badge badge-primary"> {{ __('Pending') }} </span> @elseif($row->status == 3) <span class="badge badge-warning"> {{ __('Accepted') }} </span> @elseif($row->status == 0)  <span class="badge badge-danger"> {{ __('Cancelled') }} </span> @endif</td>
              <td>@if($row->payment_status == 1) <span class="badge badge-success">{{ __('Completed') }}</span> @elseif($row->payment_status == 0)  <span class="badge badge-danger"> {{ __('Pending') }} </span> @endif</td>
              <td>{{ $row->created_at->diffforHumans() }}</td>
              <td> <div class="dropdown d-inline">
                <button class="btn btn-primary dropdown-toggle" type="button" id="dropdownMenuButton2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                  Action
                </button>
                <div class="dropdown-menu">
                  <a class="dropdown-item has-icon" href="{{ route('store.order.show',$row->id) }}"><i class="fas fa-eye"></i> View</a>
                  <a class="dropdown-item has-icon" target="_blank" href="{{ route('store.invoice_print',$row->id) }}"><i class="fas fa-print"></i> Print Now</a>

                   <a class="dropdown-item has-icon" href="{{ route('store.invoice',$row->id) }}"><i class="fas fa-file-invoice"></i> Download Invoice</a>

                  
                </div>
              </div>
            </td>  
            </tr>
            @endforeach
          </tbody>

          <tfoot>
            <tr>
              <th>{{ __('Order ID') }}</th>
              <th>{{ __('Order Type') }}</th>
              <th>{{ __('Amount') }}</th>
              <th>{{ __('Discount') }}</th>
              <th>{{ __('Commission') }}</th>
              <th>{{ __('Status') }}</th>
              <th>{{ __('Payment Status') }}</th>
              <th>{{ __('Time') }}</th>
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
@endsection