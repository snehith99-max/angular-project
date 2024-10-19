import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnTenquiryviewComponent } from './smr-trn-tenquiryview.component';

describe('SmrTrnTenquiryviewComponent', () => {
  let component: SmrTrnTenquiryviewComponent;
  let fixture: ComponentFixture<SmrTrnTenquiryviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnTenquiryviewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnTenquiryviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
