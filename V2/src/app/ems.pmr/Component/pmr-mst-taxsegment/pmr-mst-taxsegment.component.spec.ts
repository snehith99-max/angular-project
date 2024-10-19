import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrMstTaxsegmentComponent } from './pmr-mst-taxsegment.component';

describe('PmrMstTaxsegmentComponent', () => {
  let component: PmrMstTaxsegmentComponent;
  let fixture: ComponentFixture<PmrMstTaxsegmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrMstTaxsegmentComponent]
    });
    fixture = TestBed.createComponent(PmrMstTaxsegmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
