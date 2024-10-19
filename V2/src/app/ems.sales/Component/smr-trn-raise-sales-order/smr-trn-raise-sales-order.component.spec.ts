import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRaiseSalesOrderComponent } from './smr-trn-raise-sales-order.component';

describe('SmrTrnRaiseSalesOrderComponent', () => {
  let component: SmrTrnRaiseSalesOrderComponent;
  let fixture: ComponentFixture<SmrTrnRaiseSalesOrderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRaiseSalesOrderComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRaiseSalesOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
