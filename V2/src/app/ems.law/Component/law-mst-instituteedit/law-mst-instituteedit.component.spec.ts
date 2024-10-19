import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LawMstInstituteeditComponent } from './law-mst-instituteedit.component';

describe('LawMstInstituteeditComponent', () => {
  let component: LawMstInstituteeditComponent;
  let fixture: ComponentFixture<LawMstInstituteeditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LawMstInstituteeditComponent]
    });
    fixture = TestBed.createComponent(LawMstInstituteeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
