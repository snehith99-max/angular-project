import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstTaxsegmentoverseascustomerComponent } from './smr-mst-taxsegmentoverseascustomer.component';

describe('SmrMstTaxsegmentoverseascustomerComponent', () => {
  let component: SmrMstTaxsegmentoverseascustomerComponent;
  let fixture: ComponentFixture<SmrMstTaxsegmentoverseascustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstTaxsegmentoverseascustomerComponent]
    });
    fixture = TestBed.createComponent(SmrMstTaxsegmentoverseascustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
