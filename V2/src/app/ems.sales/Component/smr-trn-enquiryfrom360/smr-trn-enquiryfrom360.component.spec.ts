import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnEnquiryfrom360Component } from './smr-trn-enquiryfrom360.component';

describe('SmrTrnEnquiryfrom360Component', () => {
  let component: SmrTrnEnquiryfrom360Component;
  let fixture: ComponentFixture<SmrTrnEnquiryfrom360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnEnquiryfrom360Component]
    });
    fixture = TestBed.createComponent(SmrTrnEnquiryfrom360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
