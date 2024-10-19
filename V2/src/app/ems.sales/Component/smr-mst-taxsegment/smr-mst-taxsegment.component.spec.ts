import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstTaxsegmentComponent } from './smr-mst-taxsegment.component';

describe('SmrMstTaxsegmentComponent', () => {
  let component: SmrMstTaxsegmentComponent;
  let fixture: ComponentFixture<SmrMstTaxsegmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstTaxsegmentComponent]
    });
    fixture = TestBed.createComponent(SmrMstTaxsegmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
