import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnOrderfrom360NewComponent } from './smr-trn-orderfrom360-new.component';

describe('SmrTrnOrderfrom360NewComponent', () => {
  let component: SmrTrnOrderfrom360NewComponent;
  let fixture: ComponentFixture<SmrTrnOrderfrom360NewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnOrderfrom360NewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnOrderfrom360NewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
