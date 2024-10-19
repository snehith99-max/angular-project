import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskMstCustomerComponent } from './tsk-mst-customer.component';

describe('TskMstCustomerComponent', () => {
  let component: TskMstCustomerComponent;
  let fixture: ComponentFixture<TskMstCustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskMstCustomerComponent]
    });
    fixture = TestBed.createComponent(TskMstCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
