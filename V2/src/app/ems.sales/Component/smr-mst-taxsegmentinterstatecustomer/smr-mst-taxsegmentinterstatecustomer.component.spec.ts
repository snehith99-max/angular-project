import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstTaxsegmentinterstatecustomerComponent } from './smr-mst-taxsegmentinterstatecustomer.component';

describe('SmrMstTaxsegmentinterstatecustomerComponent', () => {
  let component: SmrMstTaxsegmentinterstatecustomerComponent;
  let fixture: ComponentFixture<SmrMstTaxsegmentinterstatecustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstTaxsegmentinterstatecustomerComponent]
    });
    fixture = TestBed.createComponent(SmrMstTaxsegmentinterstatecustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
