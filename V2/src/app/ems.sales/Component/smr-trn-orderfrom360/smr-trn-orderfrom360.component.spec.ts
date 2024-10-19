import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnOrderfrom360Component } from './smr-trn-orderfrom360.component';

describe('SmrTrnOrderfrom360Component', () => {
  let component: SmrTrnOrderfrom360Component;
  let fixture: ComponentFixture<SmrTrnOrderfrom360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnOrderfrom360Component]
    });
    fixture = TestBed.createComponent(SmrTrnOrderfrom360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
