import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RblTrnDirectinvoice360Component } from './rbl-trn-directinvoice360.component';

describe('RblTrnDirectinvoice360Component', () => {
  let component: RblTrnDirectinvoice360Component;
  let fixture: ComponentFixture<RblTrnDirectinvoice360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RblTrnDirectinvoice360Component]
    });
    fixture = TestBed.createComponent(RblTrnDirectinvoice360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
