import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstTaxsegmentunassigncustomerComponent } from './smr-mst-taxsegmentunassigncustomer.component';

describe('SmrMstTaxsegmentunassigncustomerComponent', () => {
  let component: SmrMstTaxsegmentunassigncustomerComponent;
  let fixture: ComponentFixture<SmrMstTaxsegmentunassigncustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstTaxsegmentunassigncustomerComponent]
    });
    fixture = TestBed.createComponent(SmrMstTaxsegmentunassigncustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
