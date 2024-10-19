import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstTaxsegmentwithinstatecustomerComponent } from './smr-mst-taxsegmentwithinstatecustomer.component';

describe('SmrMstTaxsegmentwithinstatecustomerComponent', () => {
  let component: SmrMstTaxsegmentwithinstatecustomerComponent;
  let fixture: ComponentFixture<SmrMstTaxsegmentwithinstatecustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstTaxsegmentwithinstatecustomerComponent]
    });
    fixture = TestBed.createComponent(SmrMstTaxsegmentwithinstatecustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
