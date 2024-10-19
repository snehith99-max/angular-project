import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnCustomer360Component } from './smr-trn-customer360.component';

describe('SmrTrnCustomer360Component', () => {
  let component: SmrTrnCustomer360Component;
  let fixture: ComponentFixture<SmrTrnCustomer360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnCustomer360Component]
    });
    fixture = TestBed.createComponent(SmrTrnCustomer360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
