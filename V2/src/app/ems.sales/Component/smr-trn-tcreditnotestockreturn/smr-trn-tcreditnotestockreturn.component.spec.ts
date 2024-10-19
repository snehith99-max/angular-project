import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnTcreditnotestockreturnComponent } from './smr-trn-tcreditnotestockreturn.component';

describe('SmrTrnTcreditnotestockreturnComponent', () => {
  let component: SmrTrnTcreditnotestockreturnComponent;
  let fixture: ComponentFixture<SmrTrnTcreditnotestockreturnComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnTcreditnotestockreturnComponent]
    });
    fixture = TestBed.createComponent(SmrTrnTcreditnotestockreturnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
