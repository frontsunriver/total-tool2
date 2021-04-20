@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=>'Payout Request'])
<div class="row">
  <div class="col-12 mt-2">
    <div class="card">
      <div class="card-body">
       <div class="float-left">
        <h5>{{ __('Total Request') }} : {{ \App\Transactions::where('status',2)->count() }}</h5>
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
                           
              <th>{{ __('Requested At') }}</th>
              <th>{{ __('View') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($requests as $row)
            <tr>
              <td><a href="{{ route('admin.payout.show',$row->id) }}">#{{ $row->id }}</a></td>
              <td>{{ $row->amount }}</td>
              <td>{{ $row->payment_mode }}</td>
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
