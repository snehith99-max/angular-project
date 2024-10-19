import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstTaxsegmentothersegmentsComponent } from './smr-mst-taxsegmentothersegments.component';

describe('SmrMstTaxsegmentothersegmentsComponent', () => {
  let component: SmrMstTaxsegmentothersegmentsComponent;
  let fixture: ComponentFixture<SmrMstTaxsegmentothersegmentsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstTaxsegmentothersegmentsComponent]
    });
    fixture = TestBed.createComponent(SmrMstTaxsegmentothersegmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
