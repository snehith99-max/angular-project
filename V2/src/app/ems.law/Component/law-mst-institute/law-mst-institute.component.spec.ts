import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LawMstInstituteComponent } from './law-mst-institute.component';

describe('LawMstInstituteComponent', () => {
  let component: LawMstInstituteComponent;
  let fixture: ComponentFixture<LawMstInstituteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LawMstInstituteComponent]
    });
    fixture = TestBed.createComponent(LawMstInstituteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
