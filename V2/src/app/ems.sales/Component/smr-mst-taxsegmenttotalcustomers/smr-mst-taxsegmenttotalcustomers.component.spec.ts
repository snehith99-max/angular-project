import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstTaxsegmenttotalcustomersComponent } from './smr-mst-taxsegmenttotalcustomers.component';

describe('SmrMstTaxsegmenttotalcustomersComponent', () => {
  let component: SmrMstTaxsegmenttotalcustomersComponent;
  let fixture: ComponentFixture<SmrMstTaxsegmenttotalcustomersComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstTaxsegmenttotalcustomersComponent]
    });
    fixture = TestBed.createComponent(SmrMstTaxsegmenttotalcustomersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
