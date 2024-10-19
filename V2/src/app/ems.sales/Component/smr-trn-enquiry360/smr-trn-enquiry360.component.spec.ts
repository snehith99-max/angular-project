import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnEnquiry360Component } from './smr-trn-enquiry360.component';

describe('SmrTrnEnquiry360Component', () => {
  let component: SmrTrnEnquiry360Component;
  let fixture: ComponentFixture<SmrTrnEnquiry360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnEnquiry360Component]
    });
    fixture = TestBed.createComponent(SmrTrnEnquiry360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
